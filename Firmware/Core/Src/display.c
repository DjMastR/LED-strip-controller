/*
 * display.c
 *
 *  Created on: 2023. máj. 7.
 *      Author: DjMastR
 */

#include "display.h"

uint8_t dot=1;

uint8_t display_TXBuffer[DIGIT_NUM] = {0};
uint8_t display_communicating = 0;

const uint8_t int_to_digit[10]={
		0b00111111,	//0
		0b00000110, //1
		0b01011011,	//2
		0b01001111, //3
		0b01100110, //4
		0b01101101, //5
		0b01111101, //6
		0b00000111, //7
		0b01111111, //8
		0b01101111, //9
};

void Latch_Enable(){
	HAL_GPIO_WritePin(Segment_LE_GPIO_Port, Segment_LE_Pin, GPIO_PIN_SET);
}

void Latch_Disable(){
	HAL_GPIO_WritePin(Segment_LE_GPIO_Port, Segment_LE_Pin, GPIO_PIN_RESET);
}

void Output_Enable(){
	HAL_GPIO_WritePin(Segment_OE_GPIO_Port, Segment_OE_Pin, GPIO_PIN_RESET);
}

void Output_Disable(){
	HAL_GPIO_WritePin(Segment_OE_GPIO_Port, Segment_OE_Pin, GPIO_PIN_SET);
}

void Display_Setup(){
	Latch_Disable();
	Output_Disable();
}

void Test_DisplayON(){
	Output_Disable();
	for(int i = 0; i< DIGIT_NUM; i++)
		display_TXBuffer[i] = 0b11111111;

	HAL_SPI_Transmit(&hspi2, display_TXBuffer, DIGIT_NUM, 10);
	Latch_Enable();
	Output_Enable();
	Latch_Disable();
}

HAL_StatusTypeDef Send_Digits(Digits digits){
	Output_Disable();
	display_TXBuffer[3] = int_to_digit[digits.First];
	display_TXBuffer[2] = int_to_digit[digits.Second];
	display_TXBuffer[1] = int_to_digit[digits.Third];
	display_TXBuffer[0] = int_to_digit[digits.Fourth];

	if(dot)
		display_TXBuffer[1] |= DP;

	HAL_StatusTypeDef stat = HAL_BUSY;
	stat = HAL_SPI_Transmit_IT(&hspi2, display_TXBuffer, DIGIT_NUM);

	if(stat != HAL_OK)
		return stat;
	Latch_Enable();
	Output_Enable();
	Latch_Disable();
	return HAL_OK;
}

void HAL_TIM_PeriodElapsedCallback(TIM_HandleTypeDef *htim){
	if(htim == &htim2){
		if(dot == 1)
			dot = 0;
		else
			dot = 1;
		Sys_WriteTime();
	}
}
