import React, { useContext, Fragment } from 'react';
import { Form, Input, Button, Row, Col, Alert } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { CoreRootStoreContext } from '../../../app/stores/aspian-core/CoreRootStore';
import { IUserFormValues } from '../../../app/models/aspian-core/user';
import { observer } from 'mobx-react-lite';
import { useTranslation } from 'react-i18next';

const Login = () => {
  const { t } = useTranslation('core_login');
  const coreRootStore = useContext(CoreRootStoreContext);
  const { login, loginError, submitting } = coreRootStore.userStore;

  const onFinish = (values: IUserFormValues) => {
    login(values);
  };

  return (
    <Row justify="center" align="middle" style={{ height: '100vh' }}>
      <Col xs={20} sm={16} md={12} lg={8} xl={6}>
        <h1>{t('title')}</h1>
        <p>{t('subTitle')}</p>
        {loginError && (
          <Fragment>
            <Alert message={loginError} type="error" />
            <br />
          </Fragment>
        )}
        <Form
          name="normal_login"
          className="login-form"
          initialValues={{ remember: true }}
          onFinish={(values) =>
            onFinish({ email: values.email, password: values.password })
          }
        >
          <Form.Item
            name="email"
            rules={[{ required: true, message: t('username-error') }]}
          >
            <Input
              prefix={<UserOutlined className="site-form-item-icon" />}
              placeholder={t('username-placeholder')}
            />
          </Form.Item>
          <Form.Item
            name="password"
            rules={[{ required: true, message: t('password-error') }]}
          >
            <Input
              prefix={<LockOutlined className="site-form-item-icon" />}
              type="password"
              placeholder={t('password-placeholder')}
            />
          </Form.Item>

          <Form.Item>
            <Button
              loading={submitting}
              type="primary"
              htmlType="submit"
              className="login-form-button"
              block
            >
              {t('login-btn')}
            </Button>
          </Form.Item>
        </Form>
      </Col>
    </Row>
  );
};

export default observer(Login);
