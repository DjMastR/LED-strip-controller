/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file           : main.h
  * @brief          : Header for main.c file.
  *                   This file contains the common defines of the application.
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

/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __MAIN_H
#define __MAIN_H

#ifdef __cplusplus
extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
#include "stm32f4xx_hal.h"

/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */
#include "clock.h"
#include "display.h"
#include "pwm.h"
#include "stdio.h"
#include "pc.h"
#include "crc.h"
/* USER CODE END Includes */

/* Exported types ------------------------------------------------------------*/
/* USER CODE BEGIN ET */
#define CRC_LENGTH 4
#define MINUTES_PER_DAY 1440
typedef enum{Hetfo, Kedd, Szerda, Csutortok, Pentek, Szombat, Vasarnap} DOW;
typedef enum{NoCommand, WriteTime, ReadTime, WriteProfile, ReadProfile, Test, TestError} Commands;
typedef enum{NoError, InterruptACK, ReadTimeError, WriteDigits, SetTime, TimeSending,
	ProfileSending, CRCError, TestingError, UnkownCommand, MissingEndChar
}ErrorType;

typedef struct{
	Commands command;
	uint8_t happend;
	HAL_StatusTypeDef status;
	ErrorType type;
}Error;
/* USER CODE END ET */

/* Exported constants --------------------------------------------------------*/
/* USER CODE BEGIN EC */
extern uint8_t refresh;
extern const char days[7][10];
extern Commands client_command;
extern uint8_t profile[MINUTES_PER_DAY];
extern uint8_t crc[CRC_LENGTH];
extern uint8_t finished_receving;
extern Error err;
/* USER CODE END EC */

/* Exported macro ------------------------------------------------------------*/
/* USER CODE BEGIN EM */

/* USER CODE END EM */

/* Exported functions prototypes ---------------------------------------------*/
void Error_Handler(void);

/* USER CODE BEGIN EFP */
void Sys_WriteTime();
/* USER CODE END EFP */

/* Private defines -----------------------------------------------------------*/
#define B1_Pin GPIO_PIN_13
#define B1_GPIO_Port GPIOC
#define Segment_LE_Pin GPIO_PIN_0
#define Segment_LE_GPIO_Port GPIOA
#define Segment_OE_Pin GPIO_PIN_1
#define Segment_OE_GPIO_Port GPIOA
#define USART_TX_Pin GPIO_PIN_2
#define USART_TX_GPIO_Port GPIOA
#define USART_RX_Pin GPIO_PIN_3
#define USART_RX_GPIO_Port GPIOA
#define Clock_INT_Pin GPIO_PIN_5
#define Clock_INT_GPIO_Port GPIOC
#define Clock_INT_EXTI_IRQn EXTI9_5_IRQn
#define TMS_Pin GPIO_PIN_13
#define TMS_GPIO_Port GPIOA
#define TCK_Pin GPIO_PIN_14
#define TCK_GPIO_Port GPIOA
#define SWO_Pin GPIO_PIN_3
#define SWO_GPIO_Port GPIOB
#define Clock_CE_Pin GPIO_PIN_6
#define Clock_CE_GPIO_Port GPIOB
/* USER CODE BEGIN Private defines */

/* USER CODE END Private defines */

#ifdef __cplusplus
}
#endif

#endif /* __MAIN_H */
