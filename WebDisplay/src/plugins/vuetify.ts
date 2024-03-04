import '@mdi/font/css/materialdesignicons.css';
import 'vuetify/styles';

import { ThemeDefinition, createVuetify } from 'vuetify';

const theme: ThemeDefinition = {
    dark: false,
    colors: {
        primary: '#602f6b'
    }
};

export default createVuetify({
    theme: {
        defaultTheme: 'theme',
        themes: {
            theme
        }
    }
});
