let messageId: number = 1;

export enum MessageType {
    auth_invalid = 'auth_invalid',
    auth_ok = 'auth_ok',
    auth_required = 'auth_required',
    event = 'event',
    pong = 'pong',
    result = 'result'
}

export interface OutgoingMessage {
    id: number | undefined;
    type: string;
}

export interface IncomingMessage {
    id: number | undefined;
    type: string;
    success: boolean | undefined;
    result: any;
}

export class AuthMessage implements OutgoingMessage {
    id = undefined;
    type: string = 'auth';
    access_token: string | null = null;

    constructor(access_token: string) {
        this.access_token = access_token;
    }
}

export class SubscribeEntitiesMessage implements OutgoingMessage {
    id = messageId++;
    type = 'subscribe_entities';
    entity_ids: string[] = [];

    constructor(entity_ids: string[]) {
        this.entity_ids = entity_ids;
    }
}

export interface EventMessage extends IncomingMessage {
    event: StatesUpdates;
}

export interface EntityState {
    /** state */
    s: string;
    /** attributes */
    a: { [key: string]: any };
    /** last_changed; if set, also applies to lu */
    lc: number;
    /** last_updated */
    lu: number;
}

export interface StatesUpdates {
    /** add */
    a?: Record<string, EntityState>;
    /** remove */
    r?: string[]; // remove
}
