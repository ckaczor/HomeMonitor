<template>
    <v-app>
        <v-app-bar
            title="Home Monitor"
            color="primary">
            <template v-slot:prepend>
                <v-app-bar-nav-icon
                    v-show="!mdAndUp"
                    @click="drawer = !drawer"></v-app-bar-nav-icon>
            </template>
        </v-app-bar>

        <v-navigation-drawer
            mobile-breakpoint="md"
            :expand-on-hover="mdAndUp"
            :rail="mdAndUp"
            :model-value="mdAndUp ? true : drawer">
            <v-list
                density="compact"
                nav>
                <v-list-item
                    prepend-icon="mdi-information-outline"
                    title="Current"
                    to="/"
                    @click="drawer = false">
                </v-list-item>
                <v-list-item
                    prepend-icon="mdi-chart-box-outline"
                    title="Summary"
                    to="summary"
                    @click="drawer = false">
                </v-list-item>
                <v-list-item
                    prepend-icon="mdi-sun-thermometer"
                    title="Outdoor"
                    to="outdoor"
                    @click="drawer = false">
                </v-list-item>
                <v-list-item
                    prepend-icon="mdi-home-analytics"
                    title="Indoor"
                    to="indoor"
                    @click="drawer = false">
                </v-list-item>
                <v-list-item
                    prepend-icon="mdi-home-lightning-bolt-outline"
                    title="Power"
                    to="power"
                    @click="drawer = false">
                </v-list-item>
            </v-list>
        </v-navigation-drawer>

        <v-main>
            <router-view />
        </v-main>
    </v-app>
</template>

<script lang="ts" setup>
    import { ref } from 'vue';
    import { useDisplay } from 'vuetify';

    const drawer = ref(false);

    const { mdAndUp } = useDisplay();
</script>
