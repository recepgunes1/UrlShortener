import { createApp } from 'vue'
import { createStore } from 'vuex'
import router from "./router/index";
import App from './App.vue'

const store = createStore({
    state(){
        return {
            number: 0,
            isPublic: true,
        }
    }
})

const app = createApp(App);
app.use(store);
app.use(router)
app.mount('#app');
