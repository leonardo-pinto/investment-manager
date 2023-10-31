import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './App.vue';
import router from './router';

import Notifications from '@kyvg/vue3-notification';
import { createVuetify } from 'vuetify';
import { VDataTable } from 'vuetify/labs/VDataTable';
import { VDatePicker } from 'vuetify/labs/VDatePicker';
import { VBtn } from 'vuetify/components/VBtn';
import * as components from 'vuetify/components';
import * as directives from 'vuetify/directives';
import 'vuetify/dist/vuetify.min.css';
import '@mdi/font/css/materialdesignicons.css';

const pinia = createPinia();
const app = createApp(App);

app.use(pinia);
app.use(router);


app.use(Notifications);

const vuetify = createVuetify({
  components: {
    ...components,
    VDataTable,
    VDatePicker,
  },
  directives,
  aliases: {
    VBtnPrimary: VBtn,
    VBtnSecondary: VBtn,
  },
  defaults: {
    VBtnPrimary: {
      color: '#00838f',
      variant: 'flat',
    },
    VBtnSecondary: {
      color: '#00838f',
      variant: 'outlined',
    },
  },
});

app.use(vuetify);

app.mount('#app');
