import { Photo } from './photo';

// Interfaces fornecem verificação de erro em tempo de compilação além de intellisense
export interface User {
    id: number;
    userName: string;
    knowAs: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    city: string;
    coutry: string;
    interests?: string;
    introduction?: string;
    lookingFor?: string;
    photos?: Photo[];
}
