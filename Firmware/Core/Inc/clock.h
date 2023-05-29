/*
 * clock.h
 *
 *  Created on: May 5, 2023
 *      Author: DjMastR
 */

#ifndef INC_CLOCK_H_
#define INC_CLOCK_H_

/* Includes ------------------------------------------------------------------*/
#include "main.h"
#include "spi.h"

extern SPI_HandleTypeDef hspi1;

/* Defines  ------------------------------------------------------------------*/

//Addresses
#define SECOND_ADDR 0x0
#define MINUTE_ADDR 0x1
#define HOUR_ADDR 0x2
#define DOW_ADDR 0x3
#define DOM_ADDR 0x4
#define MONTH_ADDR 0x5
#define YEAR_ADDR 0x6
#define OSCILLATION_ADJ_ADDR 0x7
#define CONTROL_REGISTER1_ADDR 0xE
#define CONTROL_REGISTER2_ADDR 0xF

//Default setup control words
#define CONTROL_WORD1 0b00110101
#define CONTROL_WORD2 0b00001000
#define CONTROL_LENGTH 4

//Masks and offsets for time data
#define SECONDS_FIRST_DIGIT_MASK 0b00001111
#define SECONDS_SECOND_DIGIT_MASK 0b01110000
#define SECONDS_SECOND_DIGIT_OFFSET 4

#define MINUTES_FIRST_DIGIT_MASK 0b00001111
#define MINUTES_SECOND_DIGIT_MASK 0b01110000
#define MINUTES_SECOND_DIGIT_OFFSET 4

#define HOURS_FIRST_DIGIT_MASK 0b00001111
#define HOURS_SECOND_DIGIT_MASK 0b00110000
#define HOURS_SECOND_DIGIT_OFFSET 4

#define DOM_FIRST_DIGIT_MASK 0b00001111
#define DOM_SECOND_DIGIT_MASK 0b00110000
#define DOM_SECOND_DIGIT_OFFSET 4

#define MONTHS_FIRST_DIGIT_MASK 0b00001111
#define MONTHS_SECOND_DIGIT_MASK 0b00010000
#define MONTHS_SECOND_DIGIT_OFFSET 4

#define CENTURY_MASK 0b10000000
#define CENTURY_OFFSET 7

#define YEARS_FIRST_DIGIT_MASK 0b00001111
#define YEARS_SECOND_DIGIT_MASK 0b11110000
#define YEARS_SECOND_DIGIT_OFFSET 4

//Commands
#define BURST_READ 0x4
#define BURST_WRITE 0x0
#define READ 0b1100
#define WRITE 0b1000

#define BUFFER_SIZE 7
#define CLOCK_SPI_TIMEOUT 10

//Control register setters
#define VALID_DATA 0b11101111
#define INT_ACK 0b11111011

/*Structures -------------------------------------------------------------------*/
typedef struct Time{
	uint8_t Second;
	uint8_t Minute;
	uint8_t Hour;
	uint8_t Day_of_week;
	uint8_t Day_of_month;
	uint8_t Month;
	uint8_t Year;
	uint8_t Century;

	uint8_t Second_FirstDigit;
	uint8_t Second_SecondDigit;
	uint8_t Minute_FirstDigit;
	uint8_t Minute_SecondDigit;
	uint8_t Hour_FirstDigit;
	uint8_t Hour_SecondDigit;
	uint8_t DOM_FirstDigit;
	uint8_t DOM_SecondDigit;
	uint8_t Month_FirstDigit;
	uint8_t Month_SecondDigit;
	uint8_t Year_FirstDigit;
	uint8_t Year_SecondDigit;


	uint8_t finished;
}Time;

/*Functions --------------------------------------------------------------------*/
void Clock_ChipEnable();
void Clock_ChipDisable();
HAL_StatusTypeDef Clock_SetUp();
HAL_StatusTypeDef Clock_SetTime(Time time);
HAL_StatusTypeDef Clock_ReadTime(Time* time);
uint8_t Clock_ReadControl2Register();
HAL_StatusTypeDef Clock_AckInt();
void TimeToDigits(Time* time);
void DigitsToTime(Time* time);

#endif /* INC_CLOCK_H_ */
