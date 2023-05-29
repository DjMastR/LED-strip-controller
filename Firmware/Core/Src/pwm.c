/*
 * pwm.c
 *
 *  Created on: May 8, 2023
 *      Author: DjMastR
 */

#include "pwm.h"

uint8_t setted = 0;

void Start_PWM(){
	if(!setted)
		TIM1->CCR4 = DEFAULT_DUTY_CYCLE;
	HAL_TIM_PWM_Start(&htim1, PWM_CHANNEL);
}

void Stop_PWM(){
	HAL_TIM_PWM_Stop(&htim1, PWM_CHANNEL);
}

void Set_DutyCycle(uint32_t dc){
	if(!setted)
		setted = 1;
	TIM1->CCR4 = dc;
}
