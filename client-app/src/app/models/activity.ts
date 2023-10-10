export interface Activity {
    id: string
    title: string
    date: string // we need our date to be a string because we're going to use an HTML date input and for that to render inside our form, then it needs to be a type of string.
    description: string
    category: string
    city: string
    venue: string
};