﻿const firstQuarter = document.getElementById('FirstQuarter')
const secondQuarter = document.getElementById('SecondQuarter')
const thirdQuarter = document.getElementById('ThirdQuarter')
const fourthQuarter = document.getElementById('FourthQuarter')
const form = document.getElementById('forgot_pass_form')
const errorElement = document.getElementById('error')
const btnCreate = document.getElementById('btnCreate')


form.addEventListener('submit', (e) => {   
    let sum = firstQuarter.value + secondQuarter.value + thirdQuarter.value + fourthQuarter.value;
    
    let messages = []
    if ( sum == 0) {
        
        messages.push('Введите значение')
       
    }
    if (messages.length > 0) { 
        btnCreate.dataset.content = "hjghgj";
        alert("Введите значение  по кварталам")
        e.preventDefault()       
    }    
})