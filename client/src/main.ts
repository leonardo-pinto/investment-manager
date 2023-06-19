import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import { store, key } from './store';

import BaseButton from './common/components/BaseButton.vue';
import BaseCard from './common/components/BaseCard.vue';
import BaseDialog from './common/components/BaseDialog.vue';

import { LoadingPlugin } from 'vue-loading-overlay';
import 'vue-loading-overlay/dist/css/index.css';

const app = createApp(App);

app.use(router);
app.use(store, key);

app.component('BaseButton', BaseButton);
app.component('BaseCard', BaseCard);
app.component('BaseDialog', BaseDialog);

app.use(LoadingPlugin);

app.mount('#app');
