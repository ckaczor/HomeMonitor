<template>
    <v-app>
        <v-app-bar title="Home Monitor" color="primary"></v-app-bar>

        <v-navigation-drawer expand-on-hover rail>
            <v-list density="compact" nav>
                <v-list-item
                    prepend-icon="mdi-view-dashboard"
                    title="Dashboard"
                    to="/">
                </v-list-item>
                <v-list-item
                    prepend-icon="mdi-sun-thermometer"
                    title="Outdoor"
                    to="outdoor">
                </v-list-item>
                <v-list-item
                    prepend-icon="mdi-home-analytics"
                    title="Indoor"
                    to="indoor">
                </v-list-item>
                <v-list-item
                    prepend-icon="mdi-home-lightning-bolt-outline"
                    title="Power"
                    to="power">
                </v-list-item>
            </v-list>
        </v-navigation-drawer>

        <v-main>
            <router-view />
        </v-main>
    </v-app>
</template>

<script lang="ts" setup>
    //
</script>
