/*
 * pc.h
 *
 *  Created on: 2023. máj. 17.
 *      Author: DjMastR
 */

#ifndef INC_PC_H_
#define INC_PC_H_

#include "main.h"
#include "usart.h"
#include "crc.h"

extern uint8_t finished_receiving;

#define BUFFER_LENGTH 4400
#define RECEIVE_BUFFER_LEN 15

#define COMMAND_MASK 0x80

#define TIMER_BUFFER 8
#define PROFILE_BUFFER MINUTES_PER_DAY
#define CRC_BUFFER (PROFILE_BUFFER + CRC_LENGTH)

HAL_StatusTypeDef ReceiveCommand();
HAL_StatusTypeDef SendProfile();
HAL_StatusTypeDef TestCom();
HAL_StatusTypeDef SendErrorTest();

#endif /* INC_PC_H_ */
