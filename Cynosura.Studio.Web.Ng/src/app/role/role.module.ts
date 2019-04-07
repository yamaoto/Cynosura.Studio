import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";

import { CoreModule } from "../core/core.module";
import { RoleCoreModule } from "../role-core/role-core.module";

import { RoleListComponent } from "./role-list.component";
import { RoleEditComponent } from "./role-edit.component";

@NgModule({
    declarations: [
        RoleListComponent,
        RoleEditComponent
    ],
    imports: [
        RouterModule.forChild([
            { path: "", component: RoleListComponent },
            { path: ":id", component: RoleEditComponent }
        ]),
        CoreModule,
        RoleCoreModule
    ],
    providers: [
    ]
})
export class RoleModule {

}