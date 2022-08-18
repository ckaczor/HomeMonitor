import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { TimeSpan } from 'src/app/models/time-span';

import moment from 'moment';

@Component({
    selector: 'app-time-range',
    templateUrl: './time-range.component.html',
    styleUrls: ['./time-range.component.scss']
})
export class TimeRangeComponent implements OnInit {

    @Input()
    public loading: boolean | undefined;

    private timeSpanValue: TimeSpan | undefined;

    @Input()
    public set timeSpan(val: TimeSpan | undefined) {
        this.timeSpanChange.emit(val);
        this.timeSpanValue = val;
    }

    public get timeSpan() {
        return this.timeSpanValue;
    }

    @Output()
    public timeSpanChange: EventEmitter<TimeSpan> = new EventEmitter<TimeSpan>();

    private dateValue: moment.Moment | undefined;

    @Input()
    public set date(val: moment.Moment | undefined) {
        this.dateChange.emit(val);
        this.dateValue = val;
    }

    public get date() {
        return this.dateValue;
    }

    @Output()
    public dateChange: EventEmitter<moment.Moment> = new EventEmitter<moment.Moment>();

    public timeSpanItems: { [value: number]: string } = {
        [TimeSpan.Last24Hours]: 'Last 24 hours',
        [TimeSpan.Day]: 'Day'
    };

    public timeSpans: typeof TimeSpan = TimeSpan;
    public maxDate: moment.Moment = moment().endOf('day');

    constructor() { }

    ngOnInit(): void { }

    public isSelectedDateToday(): boolean {
        const isToday = moment(this.date).startOf('day').isSame(moment().startOf('day'));

        return isToday;
    }

    public resetToToday() {
        this.date = moment().startOf('day');
    }

    public getSelectedDateDisplayString(): string {
        return moment(this.date).format('LL');
    }

    public handleDateArrowClick(value: number) {
        this.date = moment(this.date).add(value, 'day');
    }
}
