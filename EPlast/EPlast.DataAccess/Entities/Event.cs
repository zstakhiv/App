﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPlast.DataAccess.Entities
{
    public class Event
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Поле назви події обовязкове!")]
        [MaxLength(50, ErrorMessage = "Назва події не може перевищувати 50 символів")]
        public string EventName { get; set; }
        [Required(ErrorMessage = "Поле з введеними змінами обовязкове!")]
        [MaxLength(200, ErrorMessage = "Текст не може перевищувати 200 символів")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Поле питаннями обовязкове!")]
        [MaxLength(200, ErrorMessage = "Питання не можуть перевищувати 200 символів")]
        public string Questions { get; set; }
        [Required(ErrorMessage = "Вам потрібно обрати дату початку!")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EventDateStart { get; set; }
        [Required(ErrorMessage = "Вам потрібно обрати дату завершення!")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EventDateEnd { get; set; }
        [Required(ErrorMessage = "Вам потрібно обрати локацію!")]
        public string Eventlocation { get; set; }
        [Required]
        public int EventTypeID { get; set; }
        [Required(ErrorMessage = "Вам потрібно обрати категорію події!")]
        public int EventCategoryID { get; set; }
        [Required]
        public int EventStatusID { get; set; }
        [Required(ErrorMessage = "Вам потрібно обрати форму проведення!")]
        public string FormOfHolding { get; set; }
        [Required(ErrorMessage = "Вам потрібно обрати для кого створена подія!")]
        public string ForWhom { get; set; }
        [Required(ErrorMessage = "Вам потрібно вказати приблизну кількість людей!")]
        public int NumberOfPartisipants { get; set; }
        public EventType EventType { get; set; }
        public EventCategory EventCategory { get; set; }
        public EventStatus EventStatus { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<EventGallary> EventGallarys { get; set; }
        public ICollection<EventAdmin> EventAdmins { get; set; }
        public ICollection<EventAdministration> EventAdministrations { get; set; }
    }
}