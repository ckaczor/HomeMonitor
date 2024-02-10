import './main.scss';

import { useEffect, useState } from 'react';
import DashboardItem from '../dashboard-item/main';
import LaundryService from '../../services/laundry/main';
import LaundryStatus from '../../services/laundry/laundry-status';

function Laundry() {
    const [latestStatus, setLatestStatus] = useState<LaundryStatus | null>(null);

    const laundryService = new LaundryService();

    useEffect(() => {
        laundryService.getLatest().then((status) => {
            setLatestStatus(status);
        });

        laundryService.start((laundryStatus: LaundryStatus) => {
            setLatestStatus(laundryStatus);
        });
    }, []);

    return (
        <DashboardItem title="Laundry">
            <div className="laundry-current">
                {latestStatus === null && <div>Loading...</div>}
                {latestStatus !== null && (
                    <div>
                        <table>
                            <tbody>
                                <tr>
                                    <td className="laundry-current-header">Washer</td>
                                    <td className={latestStatus!.washer!.toString()}>{latestStatus!.washer ? 'On' : 'Off'}</td>
                                </tr>
                                <tr>
                                    <td className="laundry-current-header">Dryer</td>
                                    <td className={latestStatus!.dryer!.toString()}>{latestStatus!.dryer ? 'On' : 'Off'}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                )}
            </div>
        </DashboardItem>
    );
}

export default Laundry;
