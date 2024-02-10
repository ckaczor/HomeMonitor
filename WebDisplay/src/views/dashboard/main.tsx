import './main.scss';

import { useState } from 'react';
import { Container, Form, Navbar } from 'react-bootstrap';
import RGL, { WidthProvider } from 'react-grid-layout';
import CurrentWeather from '../../components/weather/current/main';
import Almanac from '../../components/almanac/main';
import Laundry from '../../components/laundry/main';
import Power from '../../components/power/main';

const ReactGridLayout = WidthProvider(RGL);

function Dashboard() {
    const [locked, setLocked] = useState(true);

    const defaultLayout = [
        { i: 'current-weather', x: 0, y: 0, w: 6, h: 7 },
        { i: 'almanac', x: 6, y: 0, w: 5, h: 7 },
        { i: 'laundry', x: 0, y: 7, w: 5, h: 5 },
        { i: 'power', x: 5, y: 7, w: 5, h: 5 },
    ];

    const storedLayout = localStorage.getItem('dashboard-layout');

    const layout = storedLayout ? JSON.parse(storedLayout) : defaultLayout;

    const onLayoutChange = (layout: RGL.Layout[]) => {
        localStorage.setItem('dashboard-layout', JSON.stringify(layout));
    };

    return (
        <Container fluid>
            <Navbar>
                <Form>
                    <Form.Check
                        id="dashboard-lock"
                        type="switch"
                        label="Locked"
                        defaultChecked={locked}
                        onChange={() => setLocked(!locked)}
                    />
                </Form>
            </Navbar>
            <ReactGridLayout
                className="layout"
                layout={layout}
                cols={20}
                rowHeight={30}
                isDraggable={!locked}
                isResizable={!locked}
                onLayoutChange={onLayoutChange}>
                <div
                    className="dashboard-item"
                    key="current-weather">
                    <CurrentWeather />
                </div>
                <div
                    className="dashboard-item"
                    key="almanac">
                    <Almanac />
                </div>
                <div
                    className="dashboard-item"
                    key="laundry">
                    <Laundry />
                </div>
                <div
                    className="dashboard-item"
                    key="power">
                    <Power />
                </div>
            </ReactGridLayout>
        </Container>
    );
}

export default Dashboard;
