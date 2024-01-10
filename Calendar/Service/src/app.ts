import 'dotenv/config';
import express from 'express';
import cors from 'cors';
import helmet from 'helmet';
import { eventsRouter } from './events/events.routes';

if (!process.env.PORT) {
    console.log('No port value specified');
    process.exit(1);
}

process.env.TZ = 'Etc/UTC';

const PORT = parseInt(process.env.PORT as string, 10);

const app = express();

app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(cors());
app.use(helmet());

app.use('/events/', eventsRouter);

app.listen(PORT, () => {
    console.log(`Server is listening on port ${PORT}`);
});