import { registerPlugins } from '@/plugins';

import App from './App.vue';

import { createApp } from 'vue';
import { createPinia } from 'pinia';

import VueApexCharts from 'vue3-apexcharts';

const pinia = createPinia();
const app = createApp(App);

registerPlugins(app);

app.use(VueApexCharts);
app.use(pinia);
app.mount('#app');
