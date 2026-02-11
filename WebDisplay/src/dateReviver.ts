export function dateReviver(key: string, value: any): any {
    if (key.indexOf('Timestamp') == -1) {
        return value;
    }

    if (typeof value === 'string' && value.match(/^\d{4}-\d\d-\d\dT\d\d:\d\d:\d\d(?:\.\d+)?(?:Z|[+-]\d\d:\d\d)?$/)) {
        const date = new Date(value);

        if (!isNaN(date.getTime())) {
            return date;
        }
    }

    return value;
}
