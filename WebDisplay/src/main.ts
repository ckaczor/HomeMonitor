import { registerPlugins } from '@/plugins';

import App from './App.vue';

import { createApp } from 'vue';
import { createPinia } from 'pinia';

import VueApexCharts from 'vue3-apexcharts';

import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css';

const pinia = createPinia();
const app = createApp(App);

registerPlugins(app);

app.use(VueApexCharts);
app.use(pinia);

app.component('VueDatePicker', VueDatePicker);

app.mount('#app');
