// AnonymousRoutePath.js
import EmailConfirmation from "../View/EmailConfirmation.js";
import Login from "../View/Login.js";

const AnonymousRoutePath = [
  {
    path: '/emailConfirmation',
    name: 'EmailConfirmation',
    component: <EmailConfirmation />,
  }
];

export default AnonymousRoutePath;
