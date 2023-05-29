/*
 * display.h
 *
 *  Created on: 2023. máj. 7.
 *      Author: DjMastR
 */

#pragma once

#ifndef INC_DISPLAY_H_
#define INC_DISPLAY_H_

#include "main.h"
#include "spi.h"



#define DIGIT_NUM 4

#define DP 0b10000000

extern const uint8_t int_to_digit[10];

typedef struct{
	uint8_t First;
	uint8_t Second;
	uint8_t Third;
	uint8_t Fourth;
}Digits;

void Test_DisplayON();
HAL_StatusTypeDef Send_Digits(Digits digits);

#endif /* INC_DISPLAY_H_ */
