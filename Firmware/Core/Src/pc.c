/*
 * pc.c
 *
 *  Created on: 2023. máj. 17.
 *      Author: DjMastR
 */

#include "pc.h"
#include "stdio.h"
#include <string.h>

uint8_t uart_buf[BUFFER_LENGTH];
uint16_t uart_len = 0;

uint8_t receive_buffer[RECEIVE_BUFFER_LEN];
uint8_t receive;
uint8_t counter = 0;
uint16_t data_counter = 0;
uint8_t saved_counter;
uint8_t received = 0;

Commands com = NoCommand;
uint8_t time_buf[TIMER_BUFFER];

HAL_StatusTypeDef SendTime(Time curr_time){
	uart_len = sprintf((char*)uart_buf, "%d;%02d;%02d;%02d;%02d;%d;%02d;%02d;%02d;%d",
		  0x80, curr_time.Century, curr_time.Year, curr_time.Month, curr_time.Day_of_month,
		  curr_time.Day_of_week, curr_time.Hour, curr_time.Minute, curr_time.Second, 0xF0);
	return HAL_UART_Transmit(&huart2, uart_buf, uart_len, 10);
}

HAL_StatusTypeDef SendProfile(){
	sprintf((char*)uart_buf, "%d;", 0x81);
	char buff[5] = {0};
	for(uint16_t i = 0; i < MINUTES_PER_DAY; i++){
		sprintf(buff, "%d;", profile[i]);
		strcat((char*)uart_buf, buff);
	}
	uint8_t* crc_array;
	crc_array = CRCToArray(xcrc32(profile, MINUTES_PER_DAY));
	for(uint8_t i = 0; i < 4; i++){
		sprintf(buff, "%d;", crc_array[i]);
		strcat((char*)uart_buf, buff);
	}
	sprintf(buff, "%d", 0xf0);
	strcat((char*)uart_buf, buff);
	uint16_t len = strlen((char*)uart_buf);
	return HAL_UART_Transmit(&huart2, uart_buf, len, 400);
}

HAL_StatusTypeDef TestCom(){
	uart_len = sprintf((char*)uart_buf, "%d;%d", 0x84, 0xf0);
	return HAL_UART_Transmit(&huart2, uart_buf, uart_len, 10);
}

HAL_StatusTypeDef SendError(Error err){
	uint8_t test = 0xff;
	if(err.happend == 2)
		test = 0xfe;
	uint8_t command = err.command;
	uint8_t status = err.status;
	uint8_t type = err.type;
	uart_len = sprintf((char*)uart_buf, "%d;%d;%d;%d;%d", test, command, status, type, 0xf0);
	return HAL_UART_Transmit(&huart2, uart_buf, uart_len, 10);
}


HAL_StatusTypeDef SendErrorTest(Time curr_time){
	Error temp;
	temp.command = curr_time.Second%6;
	temp.status = curr_time.Second%4;
	temp.type = curr_time.Second%10;
	temp.happend = 2;
	return SendError(temp);
}

Time ReceivedTime(){
	Time temp;

	temp.Century = time_buf[0];
	temp.Year = time_buf[1];
	temp.Month = time_buf[2];
	temp.Day_of_month = time_buf[3];
	temp.Day_of_week = time_buf[4];
	temp.Hour = time_buf[5];
	temp.Minute = time_buf[6];
	temp.Second = time_buf[7];

	TimeToDigits(&temp);

	return temp;
}

HAL_StatusTypeDef ReceiveCommand(){
	return HAL_UART_Receive_IT(&huart2, &receive, 1);
}

void HAL_UART_RxCpltCallback(UART_HandleTypeDef *huart){
	if(huart->Instance == USART2){
		receive_buffer[counter++] = receive;
		if(counter == RECEIVE_BUFFER_LEN)
			counter = 0;
		HAL_StatusTypeDef status = HAL_OK;
		switch(com){
		case NoCommand:
			if(receive & COMMAND_MASK){
				switch(receive){
				case 0x80:
					com = ReadTime;
					break;
				case 0x81:
					com = ReadProfile;
					break;
				case 0x84:
					com = Test;
					break;
				case 0x82:
					com = WriteTime;
					data_counter = 0;
					break;
				case 0x83:
					com = WriteProfile;
					data_counter = 0;
					break;
				case 0x8f:
					com = TestError;
					break;
				}

			}
			else{
				err.happend = 1;
				err.command = NoCommand;
				err.status = status;
				err.type = UnkownCommand;
			}
			break;
		case WriteTime:
			if(data_counter != TIMER_BUFFER)
				time_buf[data_counter++] = receive;
			else if(receive == 0xF0){
				finished_receiving = 1;
				client_command = com;
				com = NoCommand;
			}
			else{
				err.happend = 1;
				err.command = WriteTime;
				err.status = status;
				err.type = MissingEndChar;
			}
			break;
		case ReadTime:
			if(receive == 0xF0){
				finished_receiving = 1;
				client_command = com;
				com = NoCommand;
			}
			else{
				err.happend = 1;
				err.command = ReadTime;
				err.status = status;
				err.type = MissingEndChar;
			}
			break;
		case ReadProfile:
			if(receive == 0xF0){
				finished_receiving = 1;
				client_command = com;
				com = NoCommand;
			}
			else{
				err.happend = 1;
				err.command = ReadProfile;
				err.status = status;
				err.type = MissingEndChar;
			}
			break;
		case Test:
			if(receive == 0xF0){
				finished_receiving = 1;
				client_command = com;
				com = NoCommand;
			}
			else{
				err.happend = 1;
				err.command = Test;
				err.status = status;
				err.type = MissingEndChar;
			}
			break;
		case TestError:
			if(receive == 0xF0){
				finished_receiving = 1;
				client_command = com;
				com = NoCommand;
			}
			else{
				err.happend = 1;
				err.command = TestError;
				err.status = status;
				err.type = MissingEndChar;
			}
			break;
		case WriteProfile:
			if(receive == 0xF0 && data_counter == 1444){
				finished_receiving = 1;
				client_command = com;
				com = NoCommand;
			}
			else if(data_counter < PROFILE_BUFFER)
				profile[data_counter] = receive;
			else if(data_counter>=PROFILE_BUFFER && data_counter < CRC_BUFFER){
				crc[(data_counter)-PROFILE_BUFFER] = receive;
			}
			else{
				err.happend = 1;
				err.command = WriteProfile;
				err.status = status;
				err.type = MissingEndChar;
			}
			data_counter++;
			break;
		}
		ReceiveCommand();
	}
}
