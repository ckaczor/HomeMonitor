<template>
    <v-app>
        <v-navigation-drawer
            rail
            permanent>
            <v-list
                density="compact"
                nav>
                <v-list-item
                    prepend-icon="mdi-information-outline"
                    title="Current"
                    to="/">
                </v-list-item>
                <v-list-item
                    prepend-icon="mdi-chart-box-outline"
                    title="Summary"
                    to="summary">
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

</script>
