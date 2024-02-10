import './main.scss';

import { useEffect, useState } from 'react';
import DashboardItem from '../dashboard-item/main';
import PowerService from '../../services/power/main';
import PowerStatus from '../../services/power/power-status';

function Power() {
    const [latestStatus, setLatestStatus] = useState<PowerStatus | null>(null);

    const powerService = new PowerService();

    useEffect(() => {
        powerService.start((powerStatus: PowerStatus) => {
            setLatestStatus(powerStatus);
        });
    }, []);

    return (
        <DashboardItem title="Power">
            <div className="power-current">
                {latestStatus === null && <div>Loading...</div>}
                {latestStatus !== null && (
                    <div>
                        <table>
                            <tbody>
                                <tr>
                                    <td className="power-current-header">Generation</td>
                                    <td>{latestStatus!.Generation < 0 ? 0 : latestStatus!.Generation} W</td>
                                </tr>
                                <tr>
                                    <td className="power-current-header">Consumption</td>
                                    <td>{latestStatus!.Consumption < 0 ? 0 : latestStatus!.Consumption} W</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                )}
            </div>
        </DashboardItem>
    );
}

export default Power;
