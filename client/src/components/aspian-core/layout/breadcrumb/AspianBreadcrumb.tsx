import React, { FC, ReactElement, Fragment } from 'react';
import { Breadcrumb } from 'antd';
import { Link } from 'react-router-dom';
import { history } from '../../../..';
import { WithTranslation, withTranslation, Trans } from 'react-i18next';
import {
    DashboardOutlined,
    PushpinOutlined,
  } from '@ant-design/icons';

const AspianBreadcrumb: FC<WithTranslation> = ({ t }) => {
  // Existing routes to define
  const breadcrumbNameMap: { [key: string]: ReactElement } = {
    '/admin': <Fragment><DashboardOutlined /> <Trans>{t('breadcrumb.items.dashboard')}</Trans></Fragment>,
    '/admin/posts': <Fragment><PushpinOutlined /> <Trans>{t('breadcrumb.items.all-posts')}</Trans></Fragment>,
  };

  const pathSnippets = history.location.pathname.split('/').filter((i) => i);
  // Breadcrumb items created using routes
  const breadcrumbItems = pathSnippets.map((_, index) => {
    const url = `/${pathSnippets.slice(0, index + 1).join('/')}`;
    return (
      <Breadcrumb.Item key={url}>
        <Link to={url}>{breadcrumbNameMap[url]}</Link>
      </Breadcrumb.Item>
    );
  });

  return <Breadcrumb separator=" > " className="breadcrumb">{breadcrumbItems}</Breadcrumb>;
};

export default withTranslation()(AspianBreadcrumb);
