<script setup lang="ts">
import { ref, computed, onMounted } from "vue";
import { takvimService, Etkinlik } from "@/services/TakvimService";

// 🗓 Veriler
const currentDate = ref(new Date());
const etkinlikler = ref<Etkinlik[]>([]);
const tatilGunleri = ref<{ [key: string]: string }>({});

const showForm = ref(false);
const newEvent = ref({
  id: 0,
  baslik: "",
  tarih: "",
});

const dayNames = ["Paz", "Pzt", "Sal", "Çar", "Per", "Cum", "Cmt"];
const monthNames = [
  "Ocak",
  "Şubat",
  "Mart",
  "Nisan",
  "Mayıs",
  "Haziran",
  "Temmuz",
  "Ağustos",
  "Eylül",
  "Ekim",
  "Kasım",
  "Aralık",
];

onMounted(async () => {
  etkinlikler.value = await takvimService.getEtkinlikler();
  tatilGunleri.value = await takvimService.getTatilGunleri();
});

const currentMonth = computed(() => currentDate.value.getMonth());
const currentYear = computed(() => currentDate.value.getFullYear());

const calendarDays = computed(() => {
  const days = [];
  const totalDays = new Date(currentYear.value, currentMonth.value + 1, 0).getDate();
  const firstDayOfMonth = new Date(currentYear.value, currentMonth.value, 1).getDay();

  for (let i = 0; i < firstDayOfMonth; i++) {
    days.push({ day: "", date: "" });
  }

  for (let day = 1; day <= totalDays; day++) {
    const dateKey = `${currentMonth.value + 1}-${day}`;
    days.push({ day, date: dateKey });
  }

  return days;
});

const etkinlikVarMi = (date: string) => {
  return etkinlikler.value.some((event) => {
    const [yil, ay, gun] = event.tarih.split("-").map(Number);
    return `${ay}-${gun}` === date;
  });
};

const tatilVarMi = (date: string) => {
  return tatilGunleri.value[date] !== undefined;
};

const prevMonth = () => {
  currentDate.value = new Date(currentYear.value, currentMonth.value - 1, 1);
};
const nextMonth = () => {
  currentDate.value = new Date(currentYear.value, currentMonth.value + 1, 1);
};

const addEvent = async () => {
  const newEventData = {
    id: Date.now(),
    baslik: newEvent.value.baslik,
    tarih: newEvent.value.tarih,
  };

  await takvimService.ekleEtkinlik(newEventData);
  etkinlikler.value = await takvimService.getEtkinlikler();
  showForm.value = false;
  // Formu temizliyoruz
  newEvent.value = { id: 0, baslik: "", tarih: "" };
};

const removeEvent = async (date: string) => {
  const eventToRemove = etkinlikler.value.find((event) => {
    const [yil, ay, gun] = event.tarih.split("-").map(Number);
    return `${ay}-${gun}` === date;
  });

  if (eventToRemove) {
    takvimService.silEtkinlik(eventToRemove.id);
    etkinlikler.value = await takvimService.getEtkinlikler();
  }
};

const toggleForm = () => {
  showForm.value = !showForm.value;
};
</script>

<template>
  <div class="calendar">
    <div class="calendar-header">
      <button @click="prevMonth" class="button">&lt;</button>
      <h2 style="font-size: 22px; color: #141414">
        {{ currentYear }} - {{ monthNames[currentMonth] }}
      </h2>
      <button @click="nextMonth" class="button">&gt;</button>

      <button @click="toggleForm" class="button add-button">Ekle</button>
    </div>

    <div v-if="showForm" class="event-form">
      <h3>Yeni Etkinlik Ekle</h3>
      <label for="eventType">Etkinlik Türü:</label>
      <select v-model="newEvent.type" id="eventType">
        <option value="Doğum Günü">Doğum Günü</option>
        <option value="İzin">İzin</option>
        <option value="Etkinlik">Etkinlik</option>
      </select>

      <label for="eventName">Başlık:</label>
      <input v-model="newEvent.baslik" type="text" id="eventName" placeholder="Başlık" required />

      <label for="eventDate">Tarih:</label>
      <input v-model="newEvent.tarih" type="date" id="eventDate" required />

      <button @click="addEvent" class="button">Kaydet</button>
      <button @click="toggleForm" class="button">İptal</button>
    </div>
    <div class="day-names">
      <div v-for="(day, index) in dayNames" :key="index" class="day-name">{{ day }}</div>
    </div>

    <div class="days">
      <div
        v-for="day in calendarDays"
        :key="day.date"
        class="day"
        :class="{ holiday: tatilVarMi(day.date), 'event-day': etkinlikVarMi(day.date) }"
      >
        <span class="day-number">{{ day.day }}</span>
        <div v-if="tatilVarMi(day.date)" class="tatil">{{ tatilGunleri[day.date] }}</div>
        <div v-if="etkinlikVarMi(day.date)" class="etkinlik">
          📌 Etkinlik Var
          <button @click="removeEvent(day.date)" class="button remove-button">X</button>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.calendar-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 28px;
  font-weight: bold;
  margin-bottom: 20px;
  padding: 20px;
  margin-left: 220px;
}

.day-names {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  text-align: center;
  font-weight: bold;
  font-size: 12px;
  padding-bottom: 10px;
  margin-bottom: 2px;
  margin-left: 20px;
}

.day-name {
  margin-left: -13px;
}

.days {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 0px;
  margin-left: 10px;
  height: 1100px;
}

.day {
  padding: 10px;
  border: 1px solid #ebeaea;
  text-align: center;
  min-height: 140px;
  font-size: 16px;
  font-weight: bold;
  background-color: #f8f8f8;
  position: relative;
  margin: 0;
}

.day-number {
  position: absolute;
  top: 12px;
  padding-left: 81px;
  font-size: 12px;
  font-weight: bold;
}

.button {
  background-color: #007bff;
  color: white;
  padding: 4px 10px;
  border: none;
  cursor: pointer;
  border-radius: 8px;
  font-size: 22px;
}

.button:hover {
  background-color: #036cdc;
}

.add-button {
  font-size: 12px;
  padding: 10px 15px;
}

/*.remove-button {
  background-color: #9b9b9b;
  color: white;
  padding: 6px 12px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 9px;
  position: absolute;
  bottom: 8px;
  right: 8px;
}*/

.remove-button {
  background-color: #e1e1e1;
  color: rgb(97, 95, 95);
  padding: 4px 7px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 9px;
  position: absolute;
  bottom: 8px;
  right: 8px;
}

.remove-button:hover {
  background-color: #b9b5b5;
}

.event-form {
  background-color: #f8f8f8;
  padding: 20px;
  border-radius: 8px;
  margin-top: 20px;
}

.event-form label {
  font-weight: bold;
  margin-bottom: 8px;
  display: block;
}

.event-form input,
.event-form select {
  width: 100%;
  padding: 8px;
  margin-bottom: 10px;
  border-radius: 4px;
  border: 1px solid #ccc;
}

.event-form button {
  background-color: #007bff;
  color: white;
  padding: 5px 10px;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  font-size: 10px;
  margin-left: 3px;
  font-weight: bold;
}

.event-form button:hover {
  background-color: #036cdc;
}
</style>
