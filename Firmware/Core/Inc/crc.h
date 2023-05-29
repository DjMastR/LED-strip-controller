/*
 * crc.h
 *
 *  Created on: 2023. máj. 23.
 *      Author: DjMastR
 */

#ifndef INC_CRC_H_
#define INC_CRC_H_

#include "main.h"

uint32_t xcrc32 (const unsigned char *buf, uint16_t len);
uint32_t ArrayToCRC(const unsigned char *buf, uint16_t len);
uint8_t* CRCToArray(uint32_t crc_num);
#endif /* INC_CRC_H_ */
