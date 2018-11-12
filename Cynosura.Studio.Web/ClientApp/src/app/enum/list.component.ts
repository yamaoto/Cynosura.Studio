import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";

import { Enum } from "../enum-core/enum.model";
import { EnumService } from "../enum-core/enum.service";

import { ModalHelper } from "../core/modal.helper";
import { StoreService } from "../core/store.service";
import { Error } from "../core/error.model";
import { Page } from "../core/page.model";

@Component({
    selector: "enum-list",
    templateUrl: "./list.component.html"
})
export class EnumListComponent implements OnInit {
    content: Page<Enum>;
    error: Error;
    pageSize = 10;
    private _pageIndex: number;
    get pageIndex(): number {
        if (!this._pageIndex) {
            this._pageIndex = this.storeService.get("enumsPageIndex") | 0;
        }
        return this._pageIndex;
    }
    set pageIndex(value: number) {
        this._pageIndex = value;
        this.storeService.set("enumsPageIndex", value);
    }
    private _solutionId: number;
    get solutionId(): number {
        if (!this._solutionId) {
            this._solutionId = this.storeService.get("enumsSolutionId") | 0;
        }
        return this._solutionId;
    }
    set solutionId(val: number) {
        this._solutionId = val;
        this.storeService.set("enumsSolutionId", this._solutionId);
        this.getEnums();
    }

    constructor(
        private modalHelper: ModalHelper,
        private enumService: EnumService,
        private router: Router,
        private route: ActivatedRoute,
        private storeService: StoreService
        ) {}

    ngOnInit(): void {
        this.getEnums();
    }

    getEnums(): void {
        if (this.solutionId) {
            this.enumService.getEnums(this.solutionId, this.pageIndex, this.pageSize)
                .then(content => {
                    this.content = content;
                })
                .catch(error => this.error = error);
        } else {
            this.content = null;
        }
    }

    reset(): void {
        this.enumService.getEnums(this.solutionId, this.content.currentPageIndex, this.pageSize)
            .then(content => { this.content = content; },
                error => this.error = error.json() as Error);
    }

    edit(id: number): void {
        this.router.navigate([id], { relativeTo: this.route, queryParams: { solutionId: this.solutionId } });
    }

    add(): void {
        this.router.navigate([0], { relativeTo: this.route, queryParams: { solutionId: this.solutionId } });
    }

    delete(id: number): void {
        this.modalHelper.confirmDelete()
            .then(() => {
                this.enumService.deleteEnum(this.solutionId, id)
                    .then(() => {
                        this.getEnums();
                    })
                    .catch(error => this.error = error);
            })
            .catch(() => { });
    }

    onPageSelected(pageIndex: number) {
        this.pageIndex = pageIndex;
        this.getEnums();
    }
}
