<script lang="ts" setup>
    import WindDirection from '@/models/weather/wind-direction';

    const props = defineProps(['windDirection']);

    function rotationClass(windDirection: WindDirection | undefined) {
        switch (windDirection) {
            case WindDirection.None:
                return 'None';
            case WindDirection.North:
                return 'N';
            case WindDirection.East:
                return 'E';
            case WindDirection.South:
                return 'S';
            case WindDirection.West:
                return 'W';
            case WindDirection.NorthEast:
                return 'NE';
            case WindDirection.SouthEast:
                return 'SE';
            case WindDirection.SouthWest:
                return 'SW';
            case WindDirection.NorthWest:
                return 'NW';
            case WindDirection.NorthNorthEast:
                return 'NNE';
            case WindDirection.EastNorthEast:
                return 'ENE';
            case WindDirection.EastSouthEast:
                return 'ESE';
            case WindDirection.SouthSouthEast:
                return 'SSE';
            case WindDirection.SouthSouthWest:
                return 'SSW';
            case WindDirection.WestSouthWest:
                return 'WSW';
            case WindDirection.WestNorthWest:
                return 'WNW';
            case WindDirection.NorthNorthWest:
                return 'NNW';
        }
        return windDirection!.toString();
    }
</script>

<template>
    <span
        class="wind-direction-arrow"
        :class="rotationClass(props.windDirection)"
        :title="props.windDirection">
        ➳
    </span>
</template>

<style scoped>
    .wind-direction-arrow {
        display: inline-block;
    }

    .None {
        display: none;
    }

    .N {
        transform: rotate(calc(-90deg + 0deg));
    }

    .E {
        transform: rotate(calc(-90deg + 90deg));
    }

    .S {
        transform: rotate(calc(-90deg + 180deg));
    }

    .W {
        transform: rotate(calc(-90deg + 270deg));
    }

    .NE {
        transform: rotate(calc(-90deg + 45deg));
    }

    .SE {
        transform: rotate(calc(-90deg + 135deg));
    }

    .SW {
        transform: rotate(calc(-90deg + 225deg));
    }

    .NW {
        transform: rotate(calc(-90deg + 315deg));
    }

    .NNE {
        transform: rotate(calc(-90deg + 22.5deg));
    }

    .ENE {
        transform: rotate(calc(-90deg + 67.5deg));
    }

    .ESE {
        transform: rotate(calc(-90deg + 112.5deg));
    }

    .SSE {
        transform: rotate(calc(-90deg + 157.5deg));
    }

    .SSW {
        transform: rotate(calc(-90deg + 202.5deg));
    }

    .WSW {
        transform: rotate(calc(-90deg + 247.5deg));
    }

    .WNW {
        transform: rotate(calc(-90deg + 292.5deg));
    }

    .NNW {
        transform: rotate(calc(-90deg + 337.5deg));
    }
</style>
