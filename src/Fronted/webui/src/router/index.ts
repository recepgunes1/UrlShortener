import axios from "axios";
import { createRouter, createWebHistory } from "vue-router";

const router = createRouter({
    history: createWebHistory(),
    routes: []
});

router.beforeEach(async (to, from, next) => {
    if (from.path === '/' && to.path.length > 1) {
        try {
            const response = await axios.get(`${process.env.VUE_APP_API_GATEWAY_URL}/redirect${to.path}`);

            if (response.data.message && typeof response.data.message === "string") {
                window.location.href = response.data.message;
            } else {
                next({ path: '/' });
            }
        } catch (error) {
            console.error("Error fetching redirect data:", error);
            next({ path: '/' });
        }
    } else {
        next();
    }
})

export default router