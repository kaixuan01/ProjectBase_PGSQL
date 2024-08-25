// AnonymousRoutePath.js
import EmailConfirmation from "../View/EmailConfirmation.js";
// import Login from "../View/Login.js";

const AnonymousRoutePath = [
  {
    path: '/ConfirmEmail/:id',
    name: 'EmailConfirmation',
    component: <EmailConfirmation />,
  }
];

export default AnonymousRoutePath;
