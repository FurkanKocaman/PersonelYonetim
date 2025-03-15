<script>
import { ref } from "vue";

export default {
    data() {
    return {
      currentDate: new Date(),
      showModal: false,
      newName: '',
      newMonth: '',
      newDay: '',
      birthdays: {},
      holidays: { "1-1": "Yılbaşı", "4-23": "23 Nisan", "5-19": "19 Mayıs", "8-30": "30 Ağustos", "10-29": "Cumhuriyet Bayramı" },
      monthNames: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"],
      dayNames: ["Paz", "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt"]
    };
  },
  computed: {
    currentMonth() {
      return this.currentDate.getMonth();
    },
    currentYear() {
      return this.currentDate.getFullYear();
    },
    calendarDays() {
    let days = [];
    let totalDays = new Date(this.currentYear, this.currentMonth + 1, 0).getDate();
    let firstDayOfMonth = new Date(this.currentYear, this.currentMonth, 1).getDay(); 

  
    for (let i = 0; i < firstDayOfMonth; i++) {
      days.push({ day: '', date: '' }); 
    }

   
    for (let day = 1; day <= totalDays; day++) {
      let dateKey = `${this.currentMonth + 1}-${day}`;
      days.push({ day, date: dateKey });
    }

    return days;
  }
  },
  
  methods: {
    prevMonth() {
    this.currentDate = new Date(this.currentYear, this.currentMonth - 1, 1);
  },
  nextMonth() {
    this.currentDate = new Date(this.currentYear, this.currentMonth + 1, 1);
  },
    isToday(date) {
      let today = new Date();
      return today.getDate() == date.split('-')[1] && today.getMonth() + 1 == date.split('-')[0];
    },
    isHoliday(date) {
      return this.holidays[date] !== undefined;
    },
    addBirthday() {
      let dateKey = `${this.newMonth}-${this.newDay}`;
      if (this.newName && this.newMonth && this.newDay) {
        this.birthdays[dateKey] = this.newName;
        this.showModal = false;
        this.newName = '';
        this.newMonth = '';
        this.newDay = '';
      }
    }
  }
};

</script>

<template>
    <div class="calendar">
      <div class="calendar-header">
        <button @click="prevMonth" class="bg-gray-300 text-white hover:bg-gray-400 px-4 py-2 rounded" style="cursor: pointer; ">&lt;</button>
        <h2 style="font-weight: bold;">{{ monthNames[currentMonth] }} {{ currentYear }}</h2>
        <button @click="nextMonth" class="bg-gray-300 text-white hover:bg-gray-400 px-4 py-2 rounded" style="cursor: pointer;">&gt;</button>
        <button @click="showModal = true" class="bg-blue-600 text-white hover:bg-blue-700 px-4 py-2 rounded " style="cursor: pointer;">Ekle</button>
        
      </div>
      <div class="days">
        <h1 class="day-names" v-for="day in dayNames" :key="day">{{ day }}</h1>
        <div v-for="day in calendarDays" :key="day.date" class="day" 
             :class="{'current-day': isToday(day.date), 'holiday': isHoliday(day.date)}">
          <span class="day-number">{{ day.day }}</span>
          <div v-if="birthdays[day.date]" style="margin-top: 158px; background-color:#d6d6d6;  font-size:12px; color: #474747;">{{ birthdays[day.date] }} doğum günü</div>
        </div>
      </div>
      
      <div class="modal" v-if="showModal">
        <div class="modal-content">
          <h3>Doğum Günü Ekle</h3>
          <label>İsim: 
  <input v-model="newName" type="text" placeholder="İsim girin" class="input-field">
</label><br>

<label>Ay: 
  <select v-model="newMonth" class="input-field">
    <option v-for="(month, index) in monthNames" :key="index" :value="index + 1">{{ month }}</option>
  </select>
</label><br>

<label>Gün: 
  <input v-model="newDay" type="number" min="1" max="31" placeholder="Gün girin" class="input-field">
</label><br>

          <label>Gün: <input v-model="newDay" type="number" min="1" max="31"></label><br>
          <button @click="addBirthday " class="bg-blue-600 text-white hover:bg-blue-700  px-4 py-2 rounded" style="cursor: pointer;" >Ekle</button>
          <button @click="showModal  = false" class="bg-blue-600 text-white hover:bg-blue-700 px-4 py-2 rounded" style="margin-left: 10px;cursor: pointer;">Kapat</button>
        </div>
      </div>
    </div>
  </template>

<style>
.calendar {
  width: 83vw;
 height: 1400px;
  padding: 20px;
  border-radius: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
 
}
.calendar-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
  margin-left:170px;
}
.days {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 5px;
  height: 500px;
  
}
.day {
  padding: 10px;
  border: 1px solid #ccc;
  text-align: center;
  min-height: 80px;
  position: relative;
  height: 200px;
}
.day-names {
    font-size: 13px;
  text-align: center;
  font-weight: bold;
  background: none;
  border: none;
  padding-top:40px;
}
.day-number {
  position: absolute;
  top: 5px;
  left: 5px;
  
}

.current-day {
  background-color: rgb(188, 229, 243);
}
.holiday {
    background-color: rgb(188, 229, 243);
  color: white;
}
.modal {
  display: flex;
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  justify-content: center;
  align-items: center;
}
.modal-content {
  background: white;
  padding: 20px;
  border-radius: 10px;
}





.input-field {
  width: 100%;
  padding: 12px;
  margin-top: 8px;
  border-radius: 5px;
  border: 1px solid #ccc;
  font-size: 16px;
  box-sizing: border-box; 
}

.btn {
  width: 100%;
  padding: 12px;
  border-radius: 5px;
  margin-top: 15px;
  font-size: 16px;
}

</style>