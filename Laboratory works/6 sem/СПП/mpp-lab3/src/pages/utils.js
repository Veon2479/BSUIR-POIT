import { persistentAtom } from '@nanostores/persistent'

export const loginStore = persistentAtom("loginStore", "", {listen: true});

export const isLoggedStore = persistentAtom("isLoggedStore", "false", {listen: true});

export const isUserSignedIn = () => {
    return isLoggedStore.get() === "true";
}