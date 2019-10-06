import { Component, OnInit } from '@angular/core';
import { GridsterConfig, GridsterItem, GridsterItemComponentInterface } from 'angular-gridster2';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
    public options: GridsterConfig;
    public dashboard: Array<GridsterItem>;
    public locked = true;

    constructor() { }

    ngOnInit() {
        this.options = {
            itemChangeCallback: () => this.saveOptions(),
            itemResizeCallback: () => this.saveOptions(),
            resizable: {
                enabled: !this.locked
            },
            draggable: {
                enabled: !this.locked
            },
            gridType: 'fixed',
            fixedColWidth: 105,
            fixedRowHeight: 105,
            displayGrid: 'none'
        };

        this.loadOptions();
    }

    changedOptions() {
        this.options.api.optionsChanged();
    }

    removeItem(item: GridsterItem) {
        this.dashboard.splice(this.dashboard.indexOf(item), 1);
    }

    addItem() {
        this.dashboard.push({} as GridsterItem);
    }

    toggleLocked() {
        this.locked = !this.locked;

        this.options.resizable.enabled = !this.locked;
        this.options.draggable.enabled = !this.locked;

        this.changedOptions();
    }

    saveOptions() {
        localStorage.setItem('dashboard-layout', JSON.stringify(this.dashboard));
    }

    loadOptions() {
        const savedLayout = localStorage.getItem('dashboard-layout');

        const defaultLayout = [
            { cols: 3, rows: 2, y: 0, x: 0 },
            { cols: 2, rows: 1, y: 0, x: 3 },
            { cols: 1, rows: 1, y: 0, x: 5 }
        ];

        if (savedLayout == null) {
            this.dashboard = defaultLayout;
            return;
        }

        try {
            this.dashboard = JSON.parse(savedLayout);
        } catch (error) {
            this.dashboard = defaultLayout;
        }
    }
}
