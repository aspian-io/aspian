import React, { ReactElement, Fragment } from 'react';
import { Breadcrumb } from 'antd';
import { Link } from 'react-router-dom';
import { history } from '../../../..';
import { useTranslation } from 'react-i18next';
import {
    DashboardOutlined,
    PushpinOutlined,
  } from '@ant-design/icons';

const AspianBreadcrumb = () => {
  const { t } = useTranslation('core_breadcrumb');
  // Existing routes to define
  const breadcrumbNameMap: { [key: string]: ReactElement } = {
    '/admin': <Fragment><DashboardOutlined /> {t('items.dashboard')}</Fragment>,
    '/admin/posts': <Fragment><PushpinOutlined /> {t('items.posts')}</Fragment>,
    '/admin/posts/details': <Fragment>{t('items.view-post')}</Fragment>,
    '/admin/posts/add-new': <Fragment>{t('items.add-new')}</Fragment>,
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

  return <Breadcrumb className="breadcrumb">{breadcrumbItems}</Breadcrumb>;
};

export default AspianBreadcrumb;
