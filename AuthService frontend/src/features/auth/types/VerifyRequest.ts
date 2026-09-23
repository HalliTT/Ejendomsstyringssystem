export interface VerifyRequest {
    ClientId: string;
    RedirectUri: string;
    ResponseType: string;
    CodeChallenge: string;
    CodeChallengeMethod: string | null;
    State: string | null;
    Scopes: string | null;
}