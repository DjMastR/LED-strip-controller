/*
 * pwm.h
 *
 *  Created on: May 8, 2023
 *      Author: DjMastR
 */

#ifndef INC_PWM_H_
#define INC_PWM_H_

#include "tim.h"

#define PWM_CHANNEL TIM_CHANNEL_4
#define DEFAULT_DUTY_CYCLE 80

void Start_PWM();
void Stop_PWM();
void Set_DutyCycle(uint32_t dc);

#endif /* INC_PWM_H_ */
