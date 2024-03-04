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
                    title="Outside"
                    to="outside">
                </v-list-item>
                <v-list-item
                    prepend-icon="mdi-home-analytics"
                    title="Inside"
                    to="inside">
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
