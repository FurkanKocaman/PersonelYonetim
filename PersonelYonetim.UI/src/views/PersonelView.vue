<script setup lang="ts">
import { ref } from "vue";

// interface PayrollItem {
//   id: number;
//   name: string;
//   department: string;
//   status: string;
//   date: string;
// }
// interface Announcement {
//   id: number;
//   title: string;
//   date: string;
//   content: string;
// }
// interface StatItem {
//   title: string;
//   value: number;
//   icon: string;
//   color: string;
// }
const payrollItems = ref([
  { id: 1, name: "Didem Deniz", department: "Pazarlama", status: "Aktif", date: "15.03.2025",email: "didem@example.com",photo:"https://sosyalmedya.co/wp-content/uploads/2011/06/Facebook-Profil-Resimleri.jpg" },
  { id: 2, name: "Mehmet Kaya", department: "Finans", status: "Aktif", date: "16.03.2025",email: "mehmet@example.com",photo:"https://sosyalmedya.co/wp-content/uploads/2011/06/Facebook-Profil-Resimleri.jpg" },
  { id: 3, name: "Ayşe Demir", department: "İK", status: "Pasif", date: "14.03.2025",email: "ayse@example.com",photo:"https://sosyalmedya.co/wp-content/uploads/2011/06/Facebook-Profil-Resimleri.jpg" },
  { id: 4, name: "Can Yılmaz", department: "Satış", status: "Aktif", date: "17.03.2025",email: "can@example.com",photo:"https://sosyalmedya.co/wp-content/uploads/2011/06/Facebook-Profil-Resimleri.jpg" },
  { id: 5, name: "Mehmet Yılmaz", department: "Satış", status: "Aktif", date: "17.03.2025",email: "mehmet2@example.com",photo:"https://sosyalmedya.co/wp-content/uploads/2011/06/Facebook-Profil-Resimleri.jpg" },
  { id: 6, name: "Salih Türk", department: "Satış", status: "Pasif", date: "17.03.2025",email: "salih@example.com",photo:"https://sosyalmedya.co/wp-content/uploads/2011/06/Facebook-Profil-Resimleri.jpg" },
  { id: 7, name: "Mustafa Kara", department: "Satış", status: "Aktif", date: "17.03.2025",email: "mustafa@example.com",photo:"https://sosyalmedya.co/wp-content/uploads/2011/06/Facebook-Profil-Resimleri.jpg" },
]);
const showForm = ref(false);
const newPerson = ref({
  name: "",
  department: "",
  status: "Aktif",
  email: "",
  photo: "",
});

const addPerson = () => {
  if (!newPerson.value.name || !newPerson.value.department || !newPerson.value.email || !newPerson.value.photo) {
    alert("Lütfen tüm alanları doldurunuz!");
    return;
  }

  payrollItems.value.push({
    id: payrollItems.value.length + 1,
    name: newPerson.value.name,
    department: newPerson.value.department,
    status: newPerson.value.status,
    date: new Date().toLocaleDateString(),
    email: newPerson.value.email,
    photo: newPerson.value.photo,
  });

 
  newPerson.value = { name: "", department: "", status: "Aktif", email: "", photo: "" };
  showForm.value = false;
};

    




</script>

<template>
  <div class="flex h-screen relative">
   
    <main class="flex-1 p-6">
      <div class="flex justify-between items-center mb-4">
        <h2 class="text-2xl font-bold">Personel Listesi</h2>
        <button @click="showForm = true" class="bg-blue-500 text-white px-4 py-2 rounded">Ekle</button>
      </div>

     
      <div class=" p-4 rounded-lg shadow-lg">
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-5 gap-4">
          <div v-for="personel in payrollItems" :key="personel.id" class="bg-white p-4 rounded-lg shadow-md">
            <img class="w-30 h-30 mx-auto rounded-full border-2 border-gray-300 mb-3" :src="personel.photo" alt="Profil Resmi">
            <h3 class="text-lg font-bold">{{ personel.name }}</h3>
            <p class="text-gray-600">{{ personel.department }}</p>
            <p class="text-gray-600">{{ personel.email }}</p>
            <p :class="personel.status === 'Aktif' ? 'text-green-500' : 'text-red-500'">{{ personel.status }}</p>
          </div>
        </div>
      </div>
    </main>

   
    <div v-if="showForm" class="fixed inset-0 flex justify-center items-center z-50">
      
      <div class="absolute inset-0 bg-gray-900 opacity-50 backdrop-blur-sm"></div>
      
      <div class="bg-white p-6 rounded-lg shadow-lg w-96 relative z-10">
        <button @click="showForm = false" class="absolute top-2 right-2 text-gray-500 hover:text-gray-800 text-xl">✖</button>
        <h3 class="text-lg font-bold mb-2 text-center">Yeni Personel Ekle</h3>
        
        <div class="grid grid-cols-1 gap-4">
          <input v-model="newPerson.name" type="text" placeholder="İsim" class="p-2 border rounded">
          <input v-model="newPerson.department" type="text" placeholder="Departman" class="p-2 border rounded">
          <select v-model="newPerson.status" class="p-2 border rounded">
            <option value="Aktif">Aktif</option>
            <option value="Pasif">Pasif</option>
          </select>
          <input v-model="newPerson.email" type="email" placeholder="E-posta" class="p-2 border rounded">
          <input v-model="newPerson.photo" type="text" placeholder="Fotoğraf URL" class="p-2 border rounded">
        </div>
        
        <button @click="addPerson" class="bg-blue-500 text-white px-4 py-2 rounded mt-4 w-full">Tamam</button>
      </div>
    </div>
  </div>
</template>



<style>
body {
  font-family: Arial, sans-serif;
  
}
button:hover {
  background-color: #2544cc; 
  transition:  0.5s ease;
  cursor: pointer;
}

</style>