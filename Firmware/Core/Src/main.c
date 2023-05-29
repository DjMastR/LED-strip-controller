/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file           : main.c
  * @brief          : Main program body
  ******************************************************************************
  * @attention
  *
  * Copyright (c) 2023 STMicroelectronics.
  * All rights reserved.
  *
  * This software is licensed under terms that can be found in the LICENSE file
  * in the root directory of this software component.
  * If no LICENSE file comes with this software, it is provided AS-IS.
  *
  ******************************************************************************
  */
/* USER CODE END Header */
/* Includes ------------------------------------------------------------------*/
#include "main.h"
#include "spi.h"
#include "tim.h"
#include "usart.h"
#include "gpio.h"

/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */
/* USER CODE END Includes */

/* Private typedef -----------------------------------------------------------*/
/* USER CODE BEGIN PTD */

/* USER CODE END PTD */

/* Private define ------------------------------------------------------------*/
/* USER CODE BEGIN PD */
/* USER CODE END PD */

/* Private macro -------------------------------------------------------------*/
/* USER CODE BEGIN PM */

/* USER CODE END PM */

/* Private variables ---------------------------------------------------------*/

/* USER CODE BEGIN PV */
Time curr_time;
uint8_t refresh;
Commands client_command;
uint8_t finished_receiving;
uint8_t profile[MINUTES_PER_DAY];
uint8_t crc[CRC_LENGTH];
Error err;
HAL_StatusTypeDef status;

const char days[7][10] = {"Hetfo","Kedd", "Szerda", "Csutortok", "Pentek", "Szombat", "Vasarnap"};
/* USER CODE END PV */

/* Private function prototypes -----------------------------------------------*/
void SystemClock_Config(void);
/* USER CODE BEGIN PFP */

/* USER CODE END PFP */

/* Private user code ---------------------------------------------------------*/
/* USER CODE BEGIN 0 */
HAL_StatusTypeDef SendTime(Time curr_time);
Time ReceivedTime();
void SendError(Error er);

void Sys_SetPWM(){
	Set_DutyCycle(profile[60*(uint16_t)curr_time.Hour+(uint16_t)curr_time.Minute]);
}

void Sys_WriteTime(){
	Digits digits;
	digits.First = curr_time.Minute_FirstDigit;
	digits.Second = curr_time.Minute_SecondDigit;
	digits.Third = curr_time.Hour_FirstDigit;
	digits.Fourth = curr_time.Hour_SecondDigit;

	status = Send_Digits(digits);
	if(status != HAL_OK){
		err.happend = 1;
		err.status = status;
		err.type = WriteDigits;
	}
}

void Sys_ReadTime(){
	status = Clock_ReadTime(&curr_time);
	if(status != HAL_OK){
		err.happend = 1;
		err.status = status;
		err.type = ReadTimeError;
		return;
	}
	while(curr_time.finished != 1);
	Sys_WriteTime();
	Sys_SetPWM();
}
/* USER CODE END 0 */

/**
  * @brief  The application entry point.
  * @retval int
  */
int main(void)
{
  /* USER CODE BEGIN 1 */

  /* USER CODE END 1 */

  /* MCU Configuration--------------------------------------------------------*/

  /* Reset of all peripherals, Initializes the Flash interface and the Systick. */
HAL_Init();

  /* USER CODE BEGIN Init */

  /* USER CODE END Init */

  /* Configure the system clock */
  SystemClock_Config();

  /* USER CODE BEGIN SysInit */

  /* USER CODE END SysInit */

  /* Initialize all configured peripherals */
  MX_GPIO_Init();
  MX_SPI1_Init();
  MX_SPI2_Init();
  MX_TIM1_Init();
  MX_TIM2_Init();
  MX_USART2_UART_Init();
  /* USER CODE BEGIN 2 */
  Clock_SetUp();
  Start_PWM();
  HAL_TIM_Base_Start_IT(&htim2);

  refresh = 0;
  client_command = NoCommand;
  finished_receiving = 0;
  err.happend = 0;
  err.command = NoCommand;
  err.status = HAL_OK;
  err.type = NoError;
  status = HAL_OK;

  HAL_UART_MspInit(&huart2);
  ReceiveCommand();
  Sys_ReadTime();

  for(int i = 0; i<11;i++)
	  profile[15*60+i] = i*10;
  /* USER CODE END 2 */

  /* Infinite loop */
  /* USER CODE BEGIN WHILE */
  while (1)
  {
    /* USER CODE END WHILE */

    /* USER CODE BEGIN 3 */
	  if(err.happend){
		  SendError(err);
		  err.command = NoCommand;
		  err.happend = 0;
		  err.status = HAL_OK;
		  err.type = NoError;
	  }


	  if(refresh){
		  status = Clock_AckInt();
		  if(status != HAL_OK){
			  err.command = NoCommand;
			  err.happend = 1;
			  err.status = status;
			  err.type = InterruptACK;
			  continue;
		  }
		  Sys_ReadTime();

		  refresh=0;
	  }
	  //if(finished_receiving && !error){
	  if(finished_receiving && err.happend == 0){
		  switch(client_command){
		  case NoCommand:
			  break;
		  case WriteTime:
			  do{
				  status = Clock_SetTime(ReceivedTime());
			  }while(status == HAL_BUSY);
			  if(status != HAL_OK){
				  err.command = WriteTime;
				  err.happend = 1;
				  err.status = status;
				  err.type = SetTime;
			  }
			  Sys_ReadTime();
			  break;
		  case ReadTime:
			  Sys_ReadTime();
			  do{
				  status = SendTime(curr_time);
			  }while(status == HAL_BUSY);
			  if(status != HAL_OK){
				  err.command = ReadTime;
				  err.happend = 1;
				  err.status = status;
				  err.type = TimeSending;
			  }
			  break;
		  case WriteProfile:
			  if(xcrc32(profile, MINUTES_PER_DAY) == ArrayToCRC(crc, CRC_LENGTH)){
				  Sys_SetPWM();
			  }
			  else{
				  err.happend = 1;
				  err.command = WriteProfile;
				  err.status = status;
				  err.type = CRCError;
			  }

			  break;
		  case ReadProfile:
			  do{
				  status = SendProfile();
			  }while(status == HAL_BUSY);
			  if(status != HAL_OK){
				  err.happend = 1;
				  err.command = ReadProfile;
				  err.status = status;
				  err.type = ProfileSending;

			  }
			  break;
		  case Test:
			  do{
				  status = TestCom();
			  }
			  while(status == HAL_BUSY);
			  if(status != HAL_OK){
				  err.happend = 1;
				  err.command = Test;
				  err.status = status;
				  err.type = TestingError;
			  }
		  	  break;
		  case TestError:
			  do{
				  Sys_ReadTime();
				  status = SendErrorTest(curr_time);
			  }
			  while(status == HAL_BUSY);
			  if(status != HAL_OK){
				  err.happend = 1;
				  err.command = Test;
				  err.status = status;
				  err.type = TestingError;
			  }
		  	  break;
		  }
		  client_command = NoCommand;
		  finished_receiving = 0;
	  }
  }
  /* USER CODE END 3 */
}

/**
  * @brief System Clock Configuration
  * @retval None
  */
void SystemClock_Config(void)
{
  RCC_OscInitTypeDef RCC_OscInitStruct = {0};
  RCC_ClkInitTypeDef RCC_ClkInitStruct = {0};

  /** Configure the main internal regulator output voltage
  */
  __HAL_RCC_PWR_CLK_ENABLE();
  __HAL_PWR_VOLTAGESCALING_CONFIG(PWR_REGULATOR_VOLTAGE_SCALE3);

  /** Initializes the RCC Oscillators according to the specified parameters
  * in the RCC_OscInitTypeDef structure.
  */
  RCC_OscInitStruct.OscillatorType = RCC_OSCILLATORTYPE_HSI;
  RCC_OscInitStruct.HSIState = RCC_HSI_ON;
  RCC_OscInitStruct.HSICalibrationValue = RCC_HSICALIBRATION_DEFAULT;
  RCC_OscInitStruct.PLL.PLLState = RCC_PLL_ON;
  RCC_OscInitStruct.PLL.PLLSource = RCC_PLLSOURCE_HSI;
  RCC_OscInitStruct.PLL.PLLM = 16;
  RCC_OscInitStruct.PLL.PLLN = 336;
  RCC_OscInitStruct.PLL.PLLP = RCC_PLLP_DIV4;
  RCC_OscInitStruct.PLL.PLLQ = 2;
  RCC_OscInitStruct.PLL.PLLR = 2;
  if (HAL_RCC_OscConfig(&RCC_OscInitStruct) != HAL_OK)
  {
    Error_Handler();
  }

  /** Initializes the CPU, AHB and APB buses clocks
  */
  RCC_ClkInitStruct.ClockType = RCC_CLOCKTYPE_HCLK|RCC_CLOCKTYPE_SYSCLK
                              |RCC_CLOCKTYPE_PCLK1|RCC_CLOCKTYPE_PCLK2;
  RCC_ClkInitStruct.SYSCLKSource = RCC_SYSCLKSOURCE_PLLCLK;
  RCC_ClkInitStruct.AHBCLKDivider = RCC_SYSCLK_DIV1;
  RCC_ClkInitStruct.APB1CLKDivider = RCC_HCLK_DIV2;
  RCC_ClkInitStruct.APB2CLKDivider = RCC_HCLK_DIV1;

  if (HAL_RCC_ClockConfig(&RCC_ClkInitStruct, FLASH_LATENCY_2) != HAL_OK)
  {
    Error_Handler();
  }
}

/* USER CODE BEGIN 4 */

/* USER CODE END 4 */

/**
  * @brief  This function is executed in case of error occurrence.
  * @retval None
  */
void Error_Handler(void)
{
  /* USER CODE BEGIN Error_Handler_Debug */
  /* User can add his own implementation to report the HAL error return state */
  __disable_irq();
  while (1)
  {
  }
  /* USER CODE END Error_Handler_Debug */
}

#ifdef  USE_FULL_ASSERT
/**
  * @brief  Reports the name of the source file and the source line number
  *         where the assert_param error has occurred.
  * @param  file: pointer to the source file name
  * @param  line: assert_param error line source number
  * @retval None
  */
void assert_failed(uint8_t *file, uint32_t line)
{
  /* USER CODE BEGIN 6 */
  /* User can add his own implementation to report the file name and line number,
     ex: printf("Wrong parameters value: file %s on line %d\r\n", file, line) */
  /* USER CODE END 6 */
}
#endif /* USE_FULL_ASSERT */
