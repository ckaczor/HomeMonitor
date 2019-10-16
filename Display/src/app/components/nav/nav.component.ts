import { Component } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';

@Component({
    selector: 'app-nav',
    templateUrl: './nav.component.html',
    styleUrls: ['./nav.component.scss']
})
export class NavComponent {
    isHandset$: Observable<boolean> = this.breakpointObserver.observe([
        Breakpoints.HandsetLandscape,
        Breakpoints.HandsetPortrait,
        Breakpoints.Handset,
        Breakpoints.TabletLandscape,
        Breakpoints.TabletPortrait,
        Breakpoints.Tablet
    ])
        .pipe(
            map(result => result.matches),
            shareReplay()
        );

    constructor(private breakpointObserver: BreakpointObserver) { }
}
