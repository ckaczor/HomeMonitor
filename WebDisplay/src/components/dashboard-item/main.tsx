import './main.scss';

import { PropsWithChildren } from 'react';

type Props = {
    title: string;
};

function DashboardItem(props: PropsWithChildren<Props>) {
    return (
        <>
            <div className="dashboard-item-header bg-primary text-white">
                {props.title}
            </div>
            <div className="dashboard-item-content">{props.children}</div>
        </>
    );
}

export default DashboardItem;
