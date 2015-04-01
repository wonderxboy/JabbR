/// <reference path="../commonreference.ts" />
module doc {
    export class loader {
        public username: string;
        public showChat: boolean = false;
        public extraParam: any;

        constructor(public hostUrl: string, public baseUrl: string)
        {
        }

        public load(targetElement: string, docuemntId: string, loadParam: any): void {
            var defaultParam = { 'padId': docuemntId, 'host': this.hostUrl, 'baseUrl': this.baseUrl, 'showChat': this.showChat, 'userName': this.username };
            $.extend(defaultParam, this.extraParam);
            $.extend(defaultParam, loadParam);
            $(targetElement).pad(defaultParam);
        }
    }
}