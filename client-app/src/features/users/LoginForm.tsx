import { ErrorMessage, Form, Formik } from "formik";
import MyFormInput from "../activities/form/MyFormInput";
import { Button, Label } from "semantic-ui-react";
import { useStore } from "../../app/stores/store";
import { observer } from "mobx-react-lite";

export default observer(function LoginForm() {
  const { userStore } = useStore();
  return (
    <Formik
      initialValues={{ email: "", password: "", error: null }}
      onSubmit={(values, { setErrors }) =>
        userStore.login(values)
        .catch((error) => {
          setErrors({ error: "Invalid email or password" });
          console.log(error)
        })
      }
    >
        
      {({ handleSubmit, isSubmitting, errors }) => (
        <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
          <MyFormInput placeholder="Email" name="email" />
          <MyFormInput placeholder="Password" name="password" type="password" />
          <ErrorMessage
            name="error"
            render={() => (
              <Label
                style={{ marginBottom: 10 }}
                basic
                color="red"
                content={errors.error}
              />
            )}
          />
          <Button loading={isSubmitting} content="Login" type="submit" fluid />
        </Form>
      )}
    </Formik>
  );
});
