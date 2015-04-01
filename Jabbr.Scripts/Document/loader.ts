/// <reference path="../commonreference.ts" />
module doc {
    export class loader {
        public username: string;
        public showChat: boolean = false;

        constructor(public hostUrl: string, public baseUrl: string)
        {
        }

        public load(targetElement: string, docuemntId: string, jsonParam: any): void {
            var defaultParam = { 'padId': docuemntId, 'host': this.hostUrl, 'baseUrl': this.baseUrl, 'showChat': this.showChat, 'userName': this.username };
            $.extend(defaultParam, jsonParam);
            $(targetElement).pad(defaultParam);
        }
    }
}