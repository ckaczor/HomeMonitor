import WindDirectionNumber from './wind-direction-number';

export default interface WindHistoryGrouped {
    bucket: string;
    minimumSpeed: number;
    averageSpeed: number;
    maximumSpeed: number;
    averageDirection: WindDirectionNumber;
}
