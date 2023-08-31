interface Url {
    longUrl: string;
    shortPath: string;
    createdDate: string;
    lastReqestedDate: string;
    requestCounter: number;
    expireDate: string;
}

export default Url;