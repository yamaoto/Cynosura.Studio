import { Component, Input, OnInit } from "@angular/core";
import { FormBuilder, Validators } from "@angular/forms";
import { ActivatedRoute, Router, Params } from "@angular/router";
import { MatSnackBar } from "@angular/material";

import { Solution } from "../solution-core/solution.model";
import { SolutionService } from "../solution-core/solution.service";

import { Error } from "../core/error.model";
import { TemplateModel, TemplateService } from "../solution-core/template-service";


@Component({
    selector: "app-solution-edit",
    templateUrl: "./solution-edit.component.html",
    styleUrls: ["./solution-edit.component.scss"]
})
export class SolutionEditComponent implements OnInit {
    id: number;
    solutionForm = this.fb.group({
        id: [],
        name: [],
        path: [],
        templateName: [],
        templateVersion: []
    });
    solution: Solution;
    templates: TemplateModel;
    error: Error;

    constructor(
        private solutionService: SolutionService,
        private templateService: TemplateService,
        private route: ActivatedRoute,
        private router: Router,
        private fb: FormBuilder,
        private snackBar: MatSnackBar) {
    }

    ngOnInit(): void {
        this.route.params.forEach((params: Params) => {
            const id: number = params.id === "0" ? null : params.id;
            this.getSolution(id);
        });
        this.templateService.getTemplates()
            .then((templates) => this.templates = templates);
    }

    private getSolution(id: number): void {
        this.id = id;
        if (!id) {
            this.solution = new Solution();
            this.solutionForm.patchValue(this.solution);
        } else {
            this.solutionService.getSolution({ id }).then(solution => {
                this.solution = solution;
                this.solutionForm.patchValue(this.solution);
            });
        }
    }

    cancel(): void {
        window.history.back();
    }

    onSubmit(): void {
        this.saveSolution();
    }

    private saveSolution(): void {
        if (this.id) {
            this.solutionService.updateSolution(this.solutionForm.value)
                .then(
                    () => window.history.back(),
                    error => this.onError(error)
                );
        } else {
            this.solutionService.createSolution(this.solutionForm.value)
                .then(
                    () => window.history.back(),
                    error => this.onError(error)
                );
        }
    }

    onError(error: Error) {
        this.error = error;
        if (error) {
            this.snackBar.open(error.message, "Ok");
            Error.setFormErrors(this.solutionForm, error);
        }
    }
    generate(): void {
        this.error = null;
        this.solutionService.generateSolution({ id: this.solution.id })
            .then(
                () => { },
                error => this.error = error
            );
    }

    upgrade(): void {
        this.error = null;
        this.solutionService.upgradeSolution({ id: this.solution.id })
            .then(
                () => { },
                error => this.error = error
            );
    }
}
