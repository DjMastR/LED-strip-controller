/*
 * clock.c
 *
 *  Created on: May 5, 2023
 *      Author: DjMastR
 */

#include "clock.h"

uint8_t clock_TXBuffer[BUFFER_SIZE] = {0};
uint8_t rxBuffer[BUFFER_SIZE] = {0};
uint8_t clock_communicating = 0;

uint8_t command;

uint8_t century = 20;

Time* time;

void Clock_ChipEnable(){
	clock_communicating = 1;
	HAL_GPIO_WritePin(Clock_CE_GPIO_Port, Clock_CE_Pin, GPIO_PIN_SET);
}

void Clock_ChipDisable(){
	HAL_GPIO_WritePin(Clock_CE_GPIO_Port, Clock_CE_Pin, GPIO_PIN_RESET);
	clock_communicating = 0;
}

HAL_StatusTypeDef Clock_SetUp(){
	if(clock_communicating)
		return HAL_BUSY;

	HAL_StatusTypeDef status;
	Clock_ChipEnable();
	uint8_t control[CONTROL_LENGTH];
	control[0] = (CONTROL_REGISTER1_ADDR << 4) | WRITE;
	control[1] = CONTROL_WORD1;
	control[2] = (CONTROL_REGISTER2_ADDR << 4) | WRITE;
	control[3] = CONTROL_WORD2;
	status = HAL_SPI_Transmit(&hspi1, control, CONTROL_LENGTH, CLOCK_SPI_TIMEOUT);
	Clock_ChipDisable();
	return status;
}

HAL_StatusTypeDef Clock_SetTime(Time time){
	century = time.Century;
	if(clock_communicating)
		return HAL_BUSY;

	//Chip Select
	Clock_ChipEnable();

	//Irando aadat osszeallitasa
	clock_TXBuffer[0] = (time.Second_SecondDigit << SECONDS_SECOND_DIGIT_OFFSET) | time.Second_FirstDigit;
	clock_TXBuffer[1] = (time.Minute_SecondDigit << MINUTES_SECOND_DIGIT_OFFSET) | time.Minute_FirstDigit;
	clock_TXBuffer[2] = (time.Hour_SecondDigit << HOURS_SECOND_DIGIT_OFFSET) | time.Hour_FirstDigit;
	clock_TXBuffer[3] = time.Day_of_week;
	clock_TXBuffer[4] = (time.DOM_SecondDigit << DOM_SECOND_DIGIT_OFFSET) | time.DOM_FirstDigit;
	clock_TXBuffer[5] = (time.Month_SecondDigit << MONTHS_SECOND_DIGIT_OFFSET) | time.Month_FirstDigit;
	clock_TXBuffer[6] = (time.Year_SecondDigit << SECONDS_SECOND_DIGIT_OFFSET) | time.Year_FirstDigit;

	//Blokkoló adatküldés a parancsnak
	HAL_StatusTypeDef status = HAL_BUSY;
	while(status == HAL_BUSY){
		command = (SECOND_ADDR << 4) | BURST_WRITE;
		status = HAL_SPI_Transmit(&hspi1, &command, 1, CLOCK_SPI_TIMEOUT);
	}
	//Ha hibatörtént kilépünk
	if(status != HAL_OK){
		Clock_ChipDisable();
		return status;
	}

	//Adat kiküldése blokkolva
	status = HAL_BUSY;
	while(status == HAL_BUSY){
		status = HAL_SPI_Transmit(&hspi1, clock_TXBuffer, BUFFER_SIZE, CLOCK_SPI_TIMEOUT);
	}
	//Chip Selecet visszavétele
	Clock_ChipDisable();
	return status;
}

HAL_StatusTypeDef Clock_ReadTime(Time* time_ptr){
	time_ptr->Century = century;
	//Ha mar folyik kommunkacio, akkor kilepunk
	if(clock_communicating)
		return HAL_BUSY;
	//Engedelyezzuk a kommunikaciot, hogy minden idozitesi eloiras biztosan teljesuljon
	Clock_ChipEnable();

	//Ido struktura lemasolasa, jelzo bit beallitasa
	time = time_ptr;
	time->finished = 0;
	//Parancs letrehozasa es kikuldese
	command = (SECOND_ADDR << 4) | BURST_READ;
	HAL_StatusTypeDef status = HAL_SPI_Transmit(&hspi1, &command, 1, CLOCK_SPI_TIMEOUT);

	//Parancs kuldes ellenorzese
	if(status != HAL_OK)
		return status;

	//Adatok fogadasa a pufferbe
	HAL_SPI_Receive_IT(&hspi1, rxBuffer, BUFFER_SIZE);
	return HAL_OK;
}

uint8_t Clock_ReadControl2Register(HAL_StatusTypeDef* status){
	if(clock_communicating){
		*status = HAL_BUSY;
		return 0xFF;
	}
	Clock_ChipEnable();
	uint8_t word;
	command = (CONTROL_REGISTER2_ADDR << 4) | READ;
	*status = HAL_SPI_Transmit(&hspi1, &command, 1, CLOCK_SPI_TIMEOUT);
	if(*status != HAL_OK)
		return 0xFF;
	*status = HAL_SPI_Receive(&hspi1, &word, 1, CLOCK_SPI_TIMEOUT);
	Clock_ChipDisable();
	return word;
}

HAL_StatusTypeDef Clock_AckInt(){
	if(clock_communicating)
		return HAL_BUSY;

	HAL_StatusTypeDef status = HAL_OK;
	uint8_t control_word;
	do{
		control_word = Clock_ReadControl2Register(&status);
	}while(status == HAL_BUSY);
	if(status != HAL_OK)
		return status;
	Clock_ChipEnable();
	control_word &= INT_ACK;
	uint8_t data[2] = {(CONTROL_REGISTER2_ADDR << 4) | WRITE, control_word};
	status = HAL_SPI_Transmit(&hspi1, data, 2, CLOCK_SPI_TIMEOUT);
	Clock_ChipDisable();
	return status;
}

void TimeToDigits(Time* time){
	//Seconds
	time->Second_SecondDigit = time->Second/10;
	time->Second_FirstDigit = time->Second - time->Second_SecondDigit*10;
	//Minutes
	time->Minute_SecondDigit = time->Minute/10;
	time->Minute_FirstDigit = time->Minute - time->Minute_SecondDigit*10;
	//Hours
	time->Hour_SecondDigit = time->Hour/10;
	time->Hour_FirstDigit = time->Hour - time->Hour_SecondDigit*10;
	//Day of the month
	time->DOM_SecondDigit = time->Day_of_month/10;
	time->DOM_FirstDigit = time->Day_of_month- time->DOM_SecondDigit*10;
	//Month
	time->Month_SecondDigit = time->Month/10;
	time->Month_FirstDigit = time->Month - time->Month_SecondDigit*10;
	//Year
	time->Year_SecondDigit = time->Year/10;
	time->Year_FirstDigit = time->Year - time->Year_SecondDigit*10;
}

void DigitsToTime(Time* time){
	//Seconds
	time->Second = time->Second_FirstDigit + time->Second_SecondDigit * 10;
	//Minutes
	time->Minute = time->Minute_FirstDigit + time->Minute_SecondDigit*10;
	//Hours
	time->Hour = time->Hour_FirstDigit + time->Hour_SecondDigit*10;
	//Day of the week
	time->Day_of_week = rxBuffer[3];
	//Day of the month
	time->Day_of_month = time->DOM_FirstDigit + time->DOM_SecondDigit * 10;
	//Month
	time->Month = time->Month_FirstDigit + time->Month_SecondDigit * 10;
	//Year
	time->Year = time->Year_FirstDigit + time->Year_SecondDigit * 10;
}

void HAL_SPI_RxCpltCallback(SPI_HandleTypeDef *hspi){
	if(hspi->Instance == SPI1){
		Clock_ChipDisable();
		//Masodpercek
		time->Second_FirstDigit = rxBuffer[0] & SECONDS_FIRST_DIGIT_MASK;
		time->Second_SecondDigit = (rxBuffer[0] & SECONDS_SECOND_DIGIT_MASK) >> SECONDS_SECOND_DIGIT_OFFSET;
		time->Second = time->Second_FirstDigit + time->Second_SecondDigit * 10;

		//Percek
		time->Minute_FirstDigit = (rxBuffer[1] & MINUTES_FIRST_DIGIT_MASK);
		time->Minute_SecondDigit = (rxBuffer[1] & MINUTES_SECOND_DIGIT_MASK) >> MINUTES_SECOND_DIGIT_OFFSET;
		time->Minute = time->Minute_FirstDigit + time->Minute_SecondDigit*10;

		//Orak
		time->Hour_FirstDigit = (rxBuffer[2] & HOURS_FIRST_DIGIT_MASK);
		time->Hour_SecondDigit = (rxBuffer[2] & HOURS_SECOND_DIGIT_MASK) >> HOURS_SECOND_DIGIT_OFFSET;
		time->Hour = time->Hour_FirstDigit + time->Hour_SecondDigit*10;

		//A het napja
		time->Day_of_week = rxBuffer[3];

		//A honap napja
		time->DOM_FirstDigit = rxBuffer[4] & DOM_FIRST_DIGIT_MASK;
		time->DOM_SecondDigit = (rxBuffer[4] & DOM_SECOND_DIGIT_MASK) >> DOM_SECOND_DIGIT_OFFSET;
		time->Day_of_month = time->DOM_FirstDigit + time->DOM_SecondDigit * 10;

		//Honap
		time->Month_FirstDigit = rxBuffer[5] & MONTHS_FIRST_DIGIT_MASK;
		time->Month_SecondDigit = (rxBuffer[5] & MONTHS_SECOND_DIGIT_MASK) >> MONTHS_SECOND_DIGIT_OFFSET;
		time->Month = time->Month_FirstDigit + time->Month_SecondDigit * 10;

		//Szazadfordulo
		 if(((rxBuffer[5] & CENTURY_MASK) >> CENTURY_OFFSET))
			 century++;

		//Ev
		time->Year_FirstDigit = (uint16_t)(rxBuffer[6] & YEARS_FIRST_DIGIT_MASK);
		time->Year_SecondDigit = (uint16_t)((rxBuffer[6] & YEARS_SECOND_DIGIT_MASK) >> YEARS_SECOND_DIGIT_OFFSET);
		time->Year = time->Year_FirstDigit + time->Year_SecondDigit * 10;

		time->finished = 1;
		time = NULL;
	}
}

void HAL_GPIO_EXTI_Callback (uint16_t GPIO_Pin){
	if(GPIO_Pin == Clock_INT_Pin){
		refresh = 1;
	}
}
